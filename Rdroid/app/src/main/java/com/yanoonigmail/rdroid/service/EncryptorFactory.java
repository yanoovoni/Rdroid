package com.yanoonigmail.rdroid.service;

import java.io.IOException;
import java.net.Socket;

/**
 * Created by Yaniv Sharon on 17/02/2016.
 */
public class EncryptorFactory {
    private Server server = Server.getInstance();
    public EncryptorFactory() {
        }

    public Encryptor createEncryptor() {
        try {
            String key = server.pureRecv();
        } catch (IOException e) {
            e.printStackTrace();
        }
        try {

        } catch (IOException e) {
            e.printStackTrace();
        }
        //todo
        return new Encryptor("01234567890123456789012345678901");
    }
}
