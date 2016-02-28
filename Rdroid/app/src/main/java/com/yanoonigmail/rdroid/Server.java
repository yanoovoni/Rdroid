package com.yanoonigmail.rdroid;

import android.content.Context;
import android.content.SharedPreferences;

import java.io.DataInputStream;
import java.io.IOException;
import java.io.PrintWriter;
import java.net.Socket;
import java.net.InetSocketAddress;
import java.net.InetAddress;


/**
 * Created by Yaniv Sharon on 17/02/2016.
 */
public class Server {
    private Socket mServerSocket;
    private InetSocketAddress mServerAddress;
    private EncryptionKeyMaker mEncryptionKeyMaker;
    private Encryptor mEncryptor;
    private static Server ourInstance = new Server();

    public static Server getInstance() {
        return ourInstance;
    }

    private Server() {
        InetAddress ip_address = InetAddress.getLoopbackAddress();
        mServerAddress = new InetSocketAddress(ip_address, 9000);
        mServerSocket = new Socket();
        mEncryptionKeyMaker = new EncryptionKeyMaker();
        connect();
    }

    /**
     * Connects to the server.
     */
    public void connect() {
        boolean connected = false;
        while (!connected) {
            connected = true;
            try {
                mServerSocket.connect(mServerAddress);
            } catch (IOException e) {
                connected = false;
                e.printStackTrace();
            }
        }
        mEncryptor = mEncryptionKeyMaker.createEncryptor(mServerSocket);
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

    public Boolean tryLogin(String email, String password) {
        if (!isEmailValid(email) || !isPassswordVaild(password)) {
            return false;
        }
        String login_request = Protocol.loginRequest(email, password);
        send(login_request);
        String login_response = recv();
        return Protocol.loginResponseBool(login_response);
    }

    private boolean isEmailValid(String email) {
        return email.contains("@");
    }

    private boolean isPassswordVaild(String password) {
        return (password.length() >= 4);
    }
}
