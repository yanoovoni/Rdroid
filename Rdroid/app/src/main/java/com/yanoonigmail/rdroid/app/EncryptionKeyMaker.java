package com.yanoonigmail.rdroid.app;

import java.net.Socket;

/**
 * Created by Yaniv Sharon on 17/02/2016.
 */
public class EncryptionKeyMaker {
    public EncryptionKeyMaker() {
        }

    public Encryptor createEncryptor(Socket server) {
        //to do
        return new Encryptor("01234567890123456789012345678901");
    }
}
