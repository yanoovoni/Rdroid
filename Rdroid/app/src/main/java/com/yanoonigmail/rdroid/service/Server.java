package com.yanoonigmail.rdroid.service;

import android.content.Context;
import android.content.SharedPreferences;
import android.util.Log;
import android.content.res.Resources;

import com.yanoonigmail.rdroid.R;
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
import java.util.Arrays;
import 	java.util.concurrent.locks.ReentrantLock;
import android.util.Base64;

import static com.yanoonigmail.rdroid.R.string.server_address;
import static com.yanoonigmail.rdroid.R.string.user_data;


/**
 * Created by Yaniv Sharon on 17/02/2016.
 */
public class Server {
    protected Context context = ApplicationContext.getContext();
    protected boolean mInitialized = false;
    protected Socket mServerSocket;
    protected InetSocketAddress mServerAddress;
    protected EncryptorFactory mEncryptorFactory;
    protected Encryptor mEncryptor;
    protected boolean mConnected = false;
    protected boolean mLoggedIn = false;
    private static Server ourInstance = new Server();
    protected Thread mInitThread;
    protected ReentrantLock mConnectLock = new ReentrantLock();
    protected ReentrantLock mLoginLock = new ReentrantLock();
    protected String mEmail;
    protected String mPassword;
    protected static Resources resources = ApplicationContext.getContext().getResources();
    protected boolean mFirstConnectTry = true;

    public static Server getInstance() {
        return ourInstance;
    }

    protected Server() {
        mInitThread = new Thread(new Runnable() {
            public void run() {
                mServerAddress = new InetSocketAddress(resources.getString(server_address), 9000);
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
                        while (!mInitialized) {
                            try {
                                Thread.sleep(10);
                            } catch (InterruptedException e) {
                                e.printStackTrace();
                            }
                        }
                        mLoggedIn = false;
                        while (!mConnected) {
                            try {
                                mServerSocket = new Socket();
                                mServerSocket.connect(mServerAddress, 5000);
                                mConnected = true;
                                mFirstConnectTry = false;
                            } catch (IOException e) {
                                e.printStackTrace();
                                try {
                                    mFirstConnectTry = false;
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
        });
        manageTasksThread.start();
    }

    protected void passiveLogin() {
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
            encrypted_message = Protocol.addMessageLen(encrypted_message);
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

    public boolean unencryptedSend(byte[] message_bytes){
        try {
            String message = Base64.encodeToString(message_bytes, Base64.DEFAULT);
            message = Protocol.addMessageLen(message);
            rawSend(message);
        } catch (Exception e) {
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
            String[] lenAndMessageArray = Protocol.cutMessageLen(encrypted_message);
            int len = Integer.parseInt(lenAndMessageArray[0]);
            encrypted_message = lenAndMessageArray[1];
            while (encrypted_message.length() < len) {
                encrypted_message += rawRecv();
            }
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

    public byte[] unencryptedRecv() {
        try {
            String message = rawRecv();
            String[] lenAndMessageArray = Protocol.cutMessageLen(message);
            int len = Integer.parseInt(lenAndMessageArray[0]);
            message = lenAndMessageArray[1];
            while (message.length() < len) {
                message += rawRecv();
            }
            return Base64.decode(message, Base64.DEFAULT);
        } catch (IOException e) {
            mConnected = false;
            connect();
            e.printStackTrace();
            return null;
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
        if (!isConnected()) {
            connect();
        }
        if (isConnected() && !isLoggedIn() && !mEmail.equals("") && !mPassword.equals("")) {
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
        waitForFirstLogin();
        return mLoggedIn;
    }

    protected void waitForFirstLogin() {
        while (mFirstConnectTry) {
            try {
                Thread.sleep(10);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }
}
