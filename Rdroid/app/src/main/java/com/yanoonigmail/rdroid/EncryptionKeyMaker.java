package com.yanoonigmail.rdroid;

import java.net.Socket;
import java.security.KeyPair;
import java.security.KeyPairGenerator;
import java.security.NoSuchAlgorithmException;
import java.security.PrivateKey;
import java.security.PublicKey;

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
