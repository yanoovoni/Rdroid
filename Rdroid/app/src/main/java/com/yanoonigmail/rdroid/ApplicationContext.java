package com.yanoonigmail.rdroid;

import android.app.Application;
import android.content.Context;

/**
 * Created by yanoo on 03-Mar-16.
 */
public class ApplicationContext extends Application {

    private static Context mContext;

    @Override
    public void onCreate() {
        super.onCreate();
        mContext = this;
    }

    public static Context getContext(){
        return mContext;
    }
}