package com.yanoonigmail.rdroid.service;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Binder;
import android.os.IBinder;
import android.content.Context;
import android.os.Parcel;
import android.util.Log;

import java.util.ArrayList;
import java.util.Random;

import static com.yanoonigmail.rdroid.R.string.user_data;

/**
 * Created by Yaniv on 24-Feb-16.
 */
public class TaskManager extends android.app.Service {
    private static TaskManager ourInstance = new TaskManager();
    private Server mServer;
    private Thread mManageTasksThread;
    private ArrayList<String> mIDList = new ArrayList<>();
    // Binder given to clients
    private final LocalBinder mBinder = new LocalBinder();
    // Random number generator
    private final Random mGenerator = new Random();

    public static TaskManager getInstance() {
        return ourInstance;
    }

    public TaskManager() {
    }

    @Override
    public IBinder onBind(Intent intent) {
        return mBinder;
    }

    @Override
    public void onCreate() {
        super.onCreate();
        Log.d("service init", "service is alive");
        mServer = Server.getInstance();
        manageTasks();
    }

    @Override
    public void onDestroy() {
        super.onDestroy();
    }

    public class LocalBinder extends Binder {
        TaskManager getService() {
            // Return this instance of the service so clients can call public methods.
            return TaskManager.this;
        }
        @Override
        protected boolean onTransact(int code, Parcel data, Parcel reply,
                                     int flags) {
            boolean success = false;
            switch (code) {
                case 1:
                    String[] inputStrings = new String[2];
                    data.readStringArray(inputStrings);
                    boolean[] outputBoolean = new boolean[1];
                    outputBoolean[0] = mServer.tryLogin(inputStrings[0], inputStrings[1]);
                    reply.writeBooleanArray(outputBoolean);
                    success = true;
                    break;
                case 2:
                    boolean[] bool = new boolean[1];
                    bool[0] = mServer.isLoggedIn();
                    reply.writeBooleanArray(bool);
                    success = true;
                    break;
            }
            return success;
        }
    }

    private void manageTasks() {
        mManageTasksThread = new Thread(new Runnable() {
            public void run() {
                while (true) {
                    if (mServer.isLoggedIn()) {
                        try {
                            Task task = Protocol.taskRequest(mServer.recv());
                            mIDList.add(task.getId());
                        } catch (Exception e) {
                            e.printStackTrace();
                        }
                    }
                    else {
                        try {
                            Thread.sleep(1000);
                        } catch (InterruptedException e) {
                            e.printStackTrace();
                        }
                    }
                }
            }
        });
        mManageTasksThread.start();
    }
}



