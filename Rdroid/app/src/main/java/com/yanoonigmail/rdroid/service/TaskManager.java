package com.yanoonigmail.rdroid.service;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Binder;
import android.os.IBinder;
import android.content.Context;
import android.os.Parcel;

import com.yanoonigmail.rdroid.app.Service;

import java.util.ArrayList;
import java.util.List;
import java.util.Random;

import static com.yanoonigmail.rdroid.R.string.user_data;

/**
 * Created by Yaniv on 24-Feb-16.
 */
public class TaskManager extends android.app.Service {
    private static TaskManager ourInstance = new TaskManager();
    private String mEmail;
    private String mPassword;
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

    @Override
    public IBinder onBind(Intent intent) {
        return mBinder;
    }

    @Override
    public void onCreate() {
        super.onCreate();
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

        /**
         * method for clients
         **/
        public int getRandomNumber() {
            return mGenerator.nextInt(100);
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
                    bool[0] = mServer.isConnected();
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
                    String task = getTask();
                    startTask(task);
                }
            }
        });
        mManageTasksThread.start();
    }

    private String getTask() {
        return "";
    }

    private String[] getNewTasks() {
        String[] IDArray = new String[mIDList.size()];
        this.mIDList.toArray(IDArray);
        mServer.send(Protocol.taskRequest(IDArray));
        Task[] taskArray = Protocol.taskResponse(mServer.recv());
        return new String[1];
    }

    private void startTask(String task) {

    }
}



