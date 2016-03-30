package com.yanoonigmail.rdroid.service;

import android.util.Log;

import java.io.IOException;
import java.net.InetSocketAddress;
import java.net.Socket;

import static com.yanoonigmail.rdroid.R.string.server_address;

/**
 * Created by 34v7 on 30/03/2016.
 */
public class ServerFileTransferor extends Server {
    protected boolean communicate = false;

    protected ServerFileTransferor() {
        mInitThread = new Thread(new Runnable() {
            public void run() {
                mServerAddress = new InetSocketAddress(resources.getString(server_address), 9002);
                mInitialized = true;
                connect();
                runCommunicationThread();
            }
        });
        mInitThread.start();
    }

    @Override
    public void connect() {
        Thread connectThread = new Thread(new Runnable() {
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
                        mEncryptor = Server.getInstance().mEncryptor;
                        mConnected = true;
                    }
                    mConnectLock.unlock();
                }
            }
        });
        connectThread.start();
    }

    public void runCommunicationThread() {
        communicate = true;
        Thread fileCommunicationThread = new Thread(new Runnable() {
            public void run() {
                while (communicate) {
                    String message = recv();
                    //todo
                }
            }
        });
        fileCommunicationThread.start();
    }

    public void close() {
        communicate = false;
    }

}
