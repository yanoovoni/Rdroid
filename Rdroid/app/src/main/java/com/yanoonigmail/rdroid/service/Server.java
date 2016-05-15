package com.yanoonigmail.rdroid.service;

import android.content.Context;
import android.content.SharedPreferences;
import android.util.Base64OutputStream;
import android.util.Log;
import android.content.res.Resources;

import com.yanoonigmail.rdroid.ApplicationContext;

import java.io.BufferedInputStream;
import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.DataOutputStream;
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.net.Socket;
import java.net.InetSocketAddress;
import java.lang.Thread;
import 	java.util.concurrent.locks.ReentrantLock;
import android.util.Base64;
import android.util.Xml;

import javax.crypto.Cipher;
import javax.crypto.CipherOutputStream;

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
    protected boolean mFirstLoginTry = true;

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
                    if (!mConnected) {
                        mLoggedIn = false;
                    }
                    while (!mConnected) {
                        try {
                            if (mServerSocket != null) {
                                mServerSocket.close();
                            }
                        } catch (IOException e2) {
                            e2.printStackTrace();
                        }
                        try {
                            mServerSocket = new Socket();
                            mServerSocket.connect(mServerAddress, 5000);
                            mConnected = true;
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
                    mConnectLock.unlock();
                    passiveLogin();
                }
            }
        });
        manageTasksThread.start();
    }

    protected void passiveLogin() {
        if (!isLoggedIn() && isConnected()) {
            SharedPreferences preferences = context.getSharedPreferences(context.getString(user_data), Context.MODE_PRIVATE);
            if (preferences.contains("email") && preferences.contains("password")) {
                mEmail = preferences.getString("email", "");
                mPassword = preferences.getString("password", "");
                tryLogin(mEmail, mPassword);
            } else {
                mFirstLoginTry = false;
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
            encrypted_message = mEncryptor.encryptFinal(message);
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
        while (!isConnected()) {
            if (!mConnectLock.isLocked()) {
                connect();
            } else {
                try {
                    Thread.sleep(100);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        }
        try {
            InputStream sis = mServerSocket.getInputStream();
            BufferedReader br = new BufferedReader(new InputStreamReader(sis));
            long messageLen = 0;
            boolean cont = false;
            char[] oneCharBuffer = new char[1];
            while (!cont) {
                if (br.read(oneCharBuffer) != 1) {
                    return "";
                }
                char oneChar = oneCharBuffer[0];
                if (oneChar == ':') {
                    cont = true;
                } else {
                    double digit = (double) Character.getNumericValue(oneChar);
                    if (digit < 0 || digit > 9) {
                        return "";
                    }
                    messageLen = (long) (messageLen * 10 + digit);
                }
            }
            char[] streamBuffer = new char[(int) Math.min(8192, messageLen)];
            int totalReadLen = 0;
            int readLen = br.read(streamBuffer);
            totalReadLen += readLen;
            String messageStart = mEncryptor.decrypt(new String(streamBuffer));
            String fileLocation = Protocol.getDownloadFileLocation(messageStart);
            if (fileLocation != null) {
                int startIndex = Protocol.getDownloadFileStartIndex(messageStart);
                if (startIndex != -1) {
                    File file = new File(fileLocation);
                    FileOutputStream fos = new FileOutputStream(file);
                    fos.write(new String(streamBuffer).getBytes("UTF-8"), startIndex, readLen - startIndex);
                    fos.flush();
                    cont = false;
                    while (!cont) {
                        streamBuffer = new char[(int) Math.min(8192, messageLen - totalReadLen)];
                        readLen = br.read(streamBuffer);
                        totalReadLen += readLen;
                        if (readLen != -1) {
                            fos.write(mEncryptor.decrypt(new String(streamBuffer)).getBytes("UTF-8"));
                            fos.flush();
                        } else {
                            cont = true;
                        }
                        if (readLen != streamBuffer.length || totalReadLen == messageLen) {
                            cont = true;
                        }
                    }
                    fos.close();
                    return "";
                }
            }
            StringBuilder stringBuilder = new StringBuilder(messageStart);
            streamBuffer = new char[(int) Math.min(8192, messageLen  - totalReadLen)];
            cont = false;
            while (!cont && totalReadLen < messageLen) {
                readLen = br.read(streamBuffer);
                totalReadLen += readLen;
                if (readLen == -1) {
                    cont = true;
                }
                stringBuilder.append(mEncryptor.decrypt(new String(streamBuffer)));
            }
            return stringBuilder.toString();
        } catch (Exception e) {
            mConnected = false;
            connect();
            e.printStackTrace();
            return "";
        }
    }

    public String rawRecv() throws IOException {
        BufferedReader input_stream = new BufferedReader(new InputStreamReader(this.mServerSocket.getInputStream()));
        return (input_stream.readLine());
    }

    public byte[] unencryptedRecv() {
        try {
            BufferedReader input_stream = new BufferedReader(new InputStreamReader(this.mServerSocket.getInputStream()));
            String message = input_stream.readLine();
            String[] lenAndMessageArray = Protocol.cutMessageLen(message);
            int len = Integer.parseInt(lenAndMessageArray[0]);
            message = lenAndMessageArray[1];
            while (message.length() < len) {
                message += input_stream.readLine();
            }
            return Base64.decode(message, Base64.DEFAULT);
        } catch (IOException e) {
            mConnected = false;
            connect();
            e.printStackTrace();
            return null;
        }
    }

    public void streamSend(InputStream stream, long streamLength, byte[] preStreamData) {
        try {
            DataOutputStream dos = new DataOutputStream(mServerSocket.getOutputStream());
            BufferedInputStream bis = new BufferedInputStream(stream);
            BufferedWriter bw = new BufferedWriter(new OutputStreamWriter(dos));
            long totalStreamLength = streamLength + preStreamData.length + 1; // pure.
            totalStreamLength = totalStreamLength + (16 - (totalStreamLength % 16)); // after AES.
            totalStreamLength = (long) (4 * Math.ceil((double) totalStreamLength / 3)); // after base64.
            bw.write(String.valueOf(totalStreamLength) + ":");
            bw.flush();
            dos.write(mEncryptor.encryptPart(new String(preStreamData)).getBytes());
            dos.flush();
            boolean again = true;
            while (again) {
                byte[] buffer = new byte[8192];
                int readLen = bis.read(buffer);
                int writeLen = readLen + (16 - (readLen % 16));
                writeLen = (int) (4 * Math.ceil((double) writeLen / 3));
                if (readLen != -1) {
                    if (readLen == buffer.length) {
                        dos.write(mEncryptor.encryptPart(new String(buffer)).getBytes(), 0, writeLen);
                    }
                    if (readLen != buffer.length) {
                        dos.write(mEncryptor.encryptFinal(new String(buffer)).getBytes(), 0, writeLen);
                        again = false;
                    }
                    dos.flush();
                } else {
                    again = false;
                }
            }
            bw.newLine();
            bw.flush();
        } catch (Exception e) {
            e.printStackTrace();
            mConnected = false;
            connect();
        }
    }

    public boolean tryLogin(String email, String password) {
        mLoginLock.lock();
        mEmail = email;
        mPassword = password;
        if (!isConnected()) {
            mConnected = false;
            connect();
        }
        if (isConnected() && !isLoggedIn() && !mEmail.equals("") && !mPassword.equals("")) {
            String login_request = Protocol.loginRequest(mEmail, mPassword);
            boolean sent = send(login_request);
            if (!sent) {
                mFirstLoginTry = false;
                return false;
            }
            String login_response = recv();
            mLoggedIn = Protocol.loginResponseBool(login_response);
        }
        mFirstLoginTry = false;
        mLoginLock.unlock();
        return mLoggedIn;
    }

    public boolean isConnected() {
        return mConnected;
    }

    public boolean isLoggedIn() {
        return mLoggedIn;
    }

    public boolean isLoggedInAfterFirstTry() {
        waitForFirstLogin();
        return isLoggedIn();
    }

    protected void waitForFirstLogin() {
        while (mFirstLoginTry) {
            try {
                Thread.sleep(10);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }

    public boolean disconnect() {
        try {
            mServerSocket.close();
            mConnected = false;
            mLoggedIn = false;
            mFirstLoginTry = true;
            connect();
            return true;
        } catch (IOException e) {
            e.printStackTrace();
            return false;
        }
    }
}
