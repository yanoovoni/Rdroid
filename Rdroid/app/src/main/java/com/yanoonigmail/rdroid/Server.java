package com.yanoonigmail.rdroid;

import android.content.Context;
import android.content.SharedPreferences;


/**
 * Created by Yaniv Sharon on 17/02/2016.
 */
public class Server {
    public Server(){
        SharedPreferences sharedPref = .getPreferences(Context.MODE_PRIVATE);
    }

    /**
     * Connects to the server.
     */
    public void connect(){

    }

    /**
     * Tries to log in to the server.
     * @param email
     * @param password
     * @return
     */
    public Integer tryLogin(String email, String password){
        if (!isEmailValid(email)){
            return ;
        }
    }

    private boolean isEmailValid(String email){
        return email.contains("@");
    }

    private boolean isPassswordVaild(String password){
        return (password.length() >= 4);
    }
}
