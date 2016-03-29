package com.yanoonigmail.rdroid.service;

import android.content.Context;
import android.content.SharedPreferences;
import android.util.Log;

import com.yanoonigmail.rdroid.ApplicationContext;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.net.Socket;
import java.net.InetSocketAddress;
import java.lang.Thread;
import 	java.util.concurrent.locks.ReentrantLock;

import static com.yanoonigmail.rdroid.R.string.user_data;


/**
 * Created by Yaniv Sharon on 17/02/2016.
 */
public class Server {
    private Context context = ApplicationContext.getContext();
    private boolean mInitialized = false;
    private Socket mServerSocket;
    private InetSocketAddress mServerAddress;
    private EncryptorFactory mEncryptorFactory;
    private Encryptor mEncryptor;
    private boolean mConnected = false;
    private boolean mLoggedIn = false;
    private static Server ourInstance = new Server();
    private Thread mInitThread;
    private ReentrantLock mConnectLock = new ReentrantLock();
    private ReentrantLock mLoginLock = new ReentrantLock();
    private String mEmail;
    private String mPassword;

    public static Server getInstance() {
        return ourInstance;
    }

    private Server() {
        mInitThread = new Thread(new Runnable() {
            public void run() {
                mServerAddress = new InetSocketAddress("79.180.166.70", 9000);
                Log.d("Server init", mServerAddress.toString());
                mEncryptorFactory = new EncryptorFactory();
                mInitialized = true;
                connect();
            }
        });
        mInitThread.start();
    }

    /**
     * Connects to the server.
     */
    public void connect() {
        Thread manageTasksThread = new Thread(new Runnable() {
            public void run() {
                if (mConnectLock.tryLock()) {
                    if (!mConnected) {
                        while (!mInitialized) {
                            try {
                                Thread.sleep(10);
                            } catch (InterruptedException e) {
                                e.printStackTrace();
                            }
                        }
                        mLoggedIn = false;
                        boolean connected = false;
                        while (!connected) {
                            try {
                                mServerSocket = new Socket();
                                mServerSocket.connect(mServerAddress, 5000);
                                connected = true;
                            } catch (IOException e) {
                                e.printStackTrace();
                                try {
                                    Thread.sleep(5000);
                                } catch (InterruptedException e2) {
                                    e2.printStackTrace();
                                }
                            }
                        }
                        try {
                            mEncryptor = mEncryptorFactory.createEncryptor();
                        } catch (Exception e) {
                            e.printStackTrace();
                        }
                        mConnected = true;
                    }
                    mConnectLock.unlock();
                    passiveLogin();
                }
            }
        });
        manageTasksThread.start();
    }

    private void passiveLogin() {
        if (!isLoggedIn()) {
            SharedPreferences preferences = context.getSharedPreferences(context.getString(user_data), Context.MODE_PRIVATE);
            if (preferences.contains("email") && preferences.contains("password")) {
                mEmail = preferences.getString("email", "");
                mPassword = preferences.getString("password", "");
                tryLogin(mEmail, mPassword);
            } else {
                SharedPreferences.OnSharedPreferenceChangeListener changeListener = new SharedPreferences.OnSharedPreferenceChangeListener() {
                    @Override
                    public void onSharedPreferenceChanged(SharedPreferences sharedPreferences, String key) {
                        passiveLogin();
                    }
                };
                preferences.registerOnSharedPreferenceChangeListener(changeListener);
            }
        }
    }

    public boolean send(String message) {
        String encrypted_message;
        try {
            encrypted_message = mEncryptor.encrypt(message);
        } catch (Exception e) {
            e.printStackTrace();
            return false;
        }
        try {
            rawSend(encrypted_message);
        } catch (IOException e) {
            mConnected = false;
            connect();
            e.printStackTrace();
            return false;
        }
        return true;
    }

    public void rawSend(String message) throws IOException{
        while (!mServerSocket.isConnected()) {
            if (!mConnectLock.isLocked()) {
                connect();
            }
            else {
                try {
                    Thread.sleep(100);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        }
        PrintWriter output_stream =
                        new PrintWriter(
                        new BufferedWriter(
                        new OutputStreamWriter(mServerSocket.getOutputStream())), true);
        output_stream.println(message);
    }

    public String recv() {
        String encrypted_message;
        while (!isConnected()) {
            if (!mConnectLock.isLocked()) {
                connect();
            }
            else {
                try {
                    Thread.sleep(100);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        }
        try {
            encrypted_message = rawRecv();
        } catch (IOException e) {
            mConnected = false;
            connect();
            e.printStackTrace();
            return "";
        }
        try {
            return mEncryptor.decrypt(encrypted_message);
        } catch (Exception e) {
            e.printStackTrace();
            return "";
        }
    }

    public String rawRecv() throws IOException {
        BufferedReader input_stream = new BufferedReader(new InputStreamReader(this.mServerSocket.getInputStream()));
        return (input_stream.readLine());
    }

    public boolean tryLogin(String email, String password) {
        mLoginLock.lock();
        mEmail = email;
        mPassword = password;
        if (isConnected() && !isLoggedIn()) {
            String login_request = Protocol.loginRequest(mEmail, mPassword);
            boolean sent = send(login_request);
            if (!sent) {
                return false;
            }
            String login_response = recv();
            mLoggedIn = Protocol.loginResponseBool(login_response);
        }
        mLoginLock.unlock();
        return mLoggedIn;
    }

    public boolean isConnected() {
        return mConnected;
    }

    public boolean isLoggedIn() {
        return mLoggedIn;
    }
}
