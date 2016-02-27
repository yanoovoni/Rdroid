package com.yanoonigmail.rdroid;

import android.content.Context;

/**
 * Created by yanoo on 27-Feb-16.
 */
public class Protocol {
    public static String line_separator = System.getProperty("line.separator");
    public static String loginRequest(String username, String password) {
        String message;
        message = new LoginActivity().getResources().getString(R.string.protocol_header);
        //to do
        return message;
    }
}
