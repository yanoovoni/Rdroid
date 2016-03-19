package com.yanoonigmail.rdroid.service;

import android.util.Log;

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


/**
 * Created by Yaniv Sharon on 17/02/2016.
 */
public class Server {
    private boolean mInitialized = false;
    private Socket mServerSocket;
    private InetSocketAddress mServerAddress;
    private EncryptorFactory mEncryptorFactory;
    private Encryptor mEncryptor;
    private boolean mConnected = false;
    private boolean mLoggedIn = false;
    private static Server ourInstance = new Server();
    private Thread mManageTasksThread;
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
                /**
                byte[] ip_address_byte_array = new byte[4];
                String ip_string = "79.179.100.134";
                String[] stringed_ip_address_byte_array = ip_string.split("\\.");
                for (int i = 0; i < stringed_ip_address_byte_array.length; i++) {
                    String stringed_byte = stringed_ip_address_byte_array[i];
                    ip_address_byte_array[i] = (byte) (Integer.valueOf(stringed_byte) & 0xFF);
                }
                try {
                    InetAddress ip_address = InetAddress.getByAddress(ip_address_byte_array);
                } catch (UnknownHostException e) {
                    e.printStackTrace();
                    Log.w("Server", "Problem with ip address. #crush");
                }
                 **/
                mServerAddress = new InetSocketAddress("79.179.100.134", 9000);
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
        mManageTasksThread = new Thread(new Runnable() {
            public void run() {
                if (mConnectLock.tryLock()) {
                    if (!mConnected) {
                        while (!mInitialized) {
                            try {
                                Thread.sleep(1000);
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
                        mEncryptor = mEncryptorFactory.createEncryptor(mServerSocket);
                        mConnected = true;
                    }
                    mConnectLock.unlock();
                }
            }
        });
        mManageTasksThread.start();
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
            pureSend(encrypted_message);
        } catch (IOException e) {
            mConnected = false;
            e.printStackTrace();
            return false;
        }
        return true;
    }

    public void pureSend(String message) throws IOException{
        if (!mServerSocket.isConnected()) {
            connect();
        }
        PrintWriter output_stream =
                        new PrintWriter(
                        new BufferedWriter(
                        new OutputStreamWriter(mServerSocket.getOutputStream())), true);
        output_stream.println(message);
    }

    public String recv() {
        String encrypted_message;
        if (!mServerSocket.isConnected()) {
            connect();
        }
        try {
            encrypted_message = pureRecv();
        } catch (IOException e) {
            mConnected = false;
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

    public String pureRecv() throws IOException {
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
