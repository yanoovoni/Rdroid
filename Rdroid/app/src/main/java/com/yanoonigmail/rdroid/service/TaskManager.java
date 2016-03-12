package com.yanoonigmail.rdroid.service;

import android.app.Service;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Binder;
import android.os.IBinder;
import android.content.Context;
import android.os.Parcel;
import android.os.RemoteException;

import com.yanoonigmail.rdroid.app.GlobalInfo;

import java.util.Random;

import static com.yanoonigmail.rdroid.R.string.user_data;

/**
 * Created by Yaniv on 24-Feb-16.
 */
public class TaskManager extends Service {
    private String mEmail;
    private String mPassword;
    private Server mServer;
    private Thread mManageTasksThread;
    // Binder given to clients
    private final LocalBinder mBinder = new LocalBinder();
    // Random number generator
    private final Random mGenerator = new Random();

    public TaskManager() {
    }

    @Override
    public IBinder onBind(Intent intent) {
        return mBinder;
    }

    @Override
    public void onCreate() {
        super.onCreate();
        GlobalInfo.getInstance().setServiceRunning(true);
        SharedPreferences preferences = getSharedPreferences (getString(user_data), Context.MODE_PRIVATE);
        connectToServer();
        if (preferences.contains("email") && preferences.contains("password")) {
            mEmail = preferences.getString("email", "");
            mPassword = preferences.getString("password", "");
            boolean logged_in = mServer.tryLogin(mEmail, mPassword);
        }
        else {
            // wait for the app to send the email and password
        }
        //manageTasks();
    }

    @Override
    public void onDestroy() {
        super.onDestroy();
        GlobalInfo.getInstance().setServiceRunning(false);
    }

    public void connectToServer() {
        mServer = Server.getInstance();
        mServer.connect();
    }

    public class LocalBinder extends Binder {
        TaskManager getService() {
            // Return this instance of the service so clients can call public methods.
            return TaskManager.this;
        }
    }

    /** method for clients **/
    public int getRandomNumber() {
        return mGenerator.nextInt(100);
    }

    @Override
    protected boolean onTransact(int code, Parcel data, Parcel reply,
                                 int flags) {
        boolean success = false;
        switch (code) {
            case 1:
                String[] strings = new String[2];
                data.readStringArray(strings);
                success = mServer.tryLogin(strings[0], strings[1]);
        }
        return success;
    }
/**
    private void manageTasks() {
        mManageTasksThread = new Thread(new Runnable() {
            public void run() {
                while (true) {
                    String[] tasks = getTasks();
                    for (String task : tasks) {
                        startTask(task);
                    }
                }
            }
        });
        mManageTasksThread.start();
    }

    private String[] getTasks() {
        mServer.send(Protocol.taskRequest());
        mServer.recv();
        Protocol.tasksResponse();
    }

    private void startTask(String task) {

    }
 **/
}



