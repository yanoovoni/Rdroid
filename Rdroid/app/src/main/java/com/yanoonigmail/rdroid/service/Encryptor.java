package com.yanoonigmail.rdroid.service;

import javax.crypto.Cipher;
import javax.crypto.KeyGenerator;
import javax.crypto.SecretKey;
import javax.crypto.spec.SecretKeySpec;

import android.util.Base64;
import android.util.Log;

import java.security.NoSuchAlgorithmException;

/**
 * Created by Yaniv Sharon on 17/02/2016.
 */
public class Encryptor {
    private String encryptionKey;

    public Encryptor(String encryptionKey)
    {
        this.encryptionKey = encryptionKey;
    }

    public Encryptor() {
        try {
            KeyGenerator keyGen = KeyGenerator.getInstance("AES");
            keyGen.init(256); // for example
            SecretKey secretKey = keyGen.generateKey();
        } catch (NoSuchAlgorithmException e) {
            e.printStackTrace();
        }
    }

    public String encrypt(String plainText) throws Exception {
        Cipher cipher = getCipher(Cipher.ENCRYPT_MODE);
        byte[] encryptedBytes = cipher.doFinal(plainText.getBytes());
        return Base64.encodeToString(encryptedBytes, Base64.DEFAULT);
    }

    public String decrypt(String encrypted) throws Exception {
        Cipher cipher = getCipher(Cipher.DECRYPT_MODE);
        byte[] encryptedBytes = Base64.decode(encrypted, Base64.DEFAULT);
        String s = new String(encryptedBytes);
        byte[] decryptedBytes = cipher.doFinal(encryptedBytes);
        return new String(decryptedBytes);
    }

    public String getEncryptionKey() {
        return this.encryptionKey;
    }

    private Cipher getCipher(int cipherMode) throws Exception {
        String encryptionAlgorithm = "AES";
        SecretKeySpec keySpecification = new SecretKeySpec(
                encryptionKey.getBytes("UTF-8"), encryptionAlgorithm);
        Cipher cipher = Cipher.getInstance(encryptionAlgorithm);
        cipher.init(cipherMode, keySpecification);
        return cipher;
    }
}
