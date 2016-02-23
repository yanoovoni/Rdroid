package com.yanoonigmail.rdroid;

import android.content.Context;
import android.content.SharedPreferences;

import java.io.IOException;
import java.net.Socket;
import java.net.InetSocketAddress;
import java.net.InetAddress;


/**
 * Created by Yaniv Sharon on 17/02/2016.
 */
public class Server {
    private Socket server_socket;
    public Server(){
    }

    /**
     * Connects to the server.
     */
    public void connect() throws IOException {
        InetAddress ip_address = InetAddress.getLoopbackAddress();
        InetSocketAddress server_address = new InetSocketAddress(ip_address , 9000);
        server_socket = new Socket();
        server_socket.connect(server_address);
    }

    /**
     * Tries to log in to the server.
     * @param email
     * @param password
     * @return
     */
    public Boolean tryLogin(String email, String password){
        if (!isEmailValid(email) || !isPassswordVaild(password)){
            return false;
        }
    }

    private boolean isEmailValid(String email){
        return email.contains("@");
    }

    private boolean isPassswordVaild(String password){
        return (password.length() >= 4);
    }
}
