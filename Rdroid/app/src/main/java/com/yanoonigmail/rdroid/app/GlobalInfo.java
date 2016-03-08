package com.yanoonigmail.rdroid.app;

/**
 * Created by 34v7 on 02/03/2016.
 */
public class GlobalInfo {
    private static GlobalInfo ourInstance = new GlobalInfo();

    public static GlobalInfo getInstance() {
        return ourInstance;
    }

    private boolean serviceRunning = false;

    private GlobalInfo() {
    }

    public boolean isServiceRunning() {
        return serviceRunning;
    }

    public void setServiceRunning(boolean serviceRunning) {
        this.serviceRunning = serviceRunning;
    }
}
