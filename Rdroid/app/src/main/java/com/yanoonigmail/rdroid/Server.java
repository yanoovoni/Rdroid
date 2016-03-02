package com.yanoonigmail.rdroid;

import android.content.Context;
import android.content.SharedPreferences;
import android.content.res.Resources;

import java.io.DataInputStream;
import java.io.IOException;
import java.io.PrintWriter;
import java.net.Socket;
import java.net.InetSocketAddress;
import java.net.InetAddress;
import java.net.UnknownHostException;
import java.lang.Thread;


/**
 * Created by Yaniv Sharon on 17/02/2016.
 */
public class Server {
    private Socket mServerSocket;
    private InetSocketAddress mServerAddress;
    private EncryptionKeyMaker mEncryptionKeyMaker;
    private Encryptor mEncryptor;
    private boolean mConnected = false;
    private boolean mLoggedIn = false;
    private static Server ourInstance = new Server();
    private Thread mManageTasksThread;

    public static Server getInstance() {
        return ourInstance;
    }

    private Server() {
        byte[] ip_address_byte_array = new byte[4];
        String ip_string = "79.182.190.155";
        String[] stringed_ip_address_byte_array = ip_string.split("\\.");
        for (int i = 0; i < stringed_ip_address_byte_array.length; i++) {
            String stringed_byte = stringed_ip_address_byte_array[i];
            ip_address_byte_array[i] = (byte) (Integer.valueOf(stringed_byte) & 0xFF);
        }
        try {
            InetAddress ip_address = InetAddress.getByAddress(ip_address_byte_array);
            mServerAddress = new InetSocketAddress(ip_address, 9001);
            mServerSocket = new Socket();
            mEncryptionKeyMaker = new EncryptionKeyMaker();
            connect();
        }
        catch (UnknownHostException e) {
            e.printStackTrace();
        }
    }

    /**
     * Connects to the server.
     */
    public void connect() {
        mManageTasksThread = new Thread(new Runnable() {
            public void run() {
                if (!mConnected) {
                    boolean connected = false;
                    while (!connected) {
                        connected = true;
                        try {
                            mServerSocket.connect(mServerAddress);
                        } catch (IOException e) {
                            connected = false;
                            e.printStackTrace();
                            try {
                                Thread.sleep(5000);
                            }
                            catch (InterruptedException e2) {
                                e2.printStackTrace();
                            }
                        }
                    }
                    mEncryptor = mEncryptionKeyMaker.createEncryptor(mServerSocket);
                    mConnected = true;
                }
            }
        });
        mManageTasksThread.start();
    }

    public void send(String message) {
        String encrypted_message;
        try {
            encrypted_message = mEncryptor.encrypt(message);
        } catch (Exception e) {
            System.out.println(e);
            return;
        }
        boolean success = false;
        while (!success) {
            success = true;
            if (!mServerSocket.isConnected()) {
                connect();
            }
            try {
                PrintWriter output_stream = new PrintWriter(mServerSocket.getOutputStream(), true);
                output_stream.print(encrypted_message);
            } catch (IOException e) {
                success = false;
                System.out.println(e);
            }
        }
    }

    public String recv() {
        DataInputStream input_stream;
        String encrypted_message;
        String message = "";
        boolean success = false;
        while (!success) {
            success = true;
            if (!mServerSocket.isConnected()) {
                connect();
            }
            try {
                input_stream = new DataInputStream(mServerSocket.getInputStream());
                encrypted_message = input_stream.readUTF();
                message = mEncryptor.decrypt(encrypted_message);
            } catch (Exception e) {
                success = false;
                System.out.println(e);
            }
        }
        return message;
    }

    public boolean tryLogin(String email, String password) {
        String login_request = Protocol.loginRequest(email, password);
        send(login_request);
        String login_response = recv();
        boolean login_response_bool = Protocol.loginResponseBool(login_response);
        mLoggedIn = login_response_bool;
        return login_response_bool;
    }

    public boolean isConnected() {
        return mConnected;
    }

    public boolean isLoggedIn() {
        return mLoggedIn;
    }
}
