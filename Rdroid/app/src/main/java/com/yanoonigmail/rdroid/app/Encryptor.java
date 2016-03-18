package com.yanoonigmail.rdroid.app;

import javax.crypto.Cipher;
import javax.crypto.spec.SecretKeySpec;

import android.util.Base64;
import android.util.Log;

/**
 * Created by Yaniv Sharon on 17/02/2016.
 */
public class Encryptor {
    private String encryptionKey;

    public Encryptor(String encryptionKey)
    {
        this.encryptionKey = encryptionKey;
    }

    public String encrypt(String plainText) throws Exception
    {
        Cipher cipher = getCipher(Cipher.ENCRYPT_MODE);
        byte[] encryptedBytes = cipher.doFinal(plainText.getBytes());
        return Base64.encodeToString(encryptedBytes, Base64.DEFAULT);
    }

    public String decrypt(String encrypted) throws Exception
    {
        Cipher cipher = getCipher(Cipher.DECRYPT_MODE);
        byte[] encryptedBytes = Base64.decode(encrypted, Base64.DEFAULT);
        String s = new String(encryptedBytes);
        byte[] decryptedBytes = cipher.doFinal(encryptedBytes);
        return new String(decryptedBytes);
    }

    private Cipher getCipher(int cipherMode)
            throws Exception
    {
        String encryptionAlgorithm = "AES";
        SecretKeySpec keySpecification = new SecretKeySpec(
                encryptionKey.getBytes("UTF-8"), encryptionAlgorithm);
        Cipher cipher = Cipher.getInstance(encryptionAlgorithm);
        cipher.init(cipherMode, keySpecification);

        return cipher;
    }
}
