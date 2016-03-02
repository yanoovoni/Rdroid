package com.yanoonigmail.rdroid;

import android.app.Service;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.IBinder;
import android.content.Context;
import android.os.SystemClock;

import static com.yanoonigmail.rdroid.R.string.user_data;

/**
 * Created by Yaniv on 24-Feb-16.
 */
public class TaskManager extends Service {
    private String mEmail;
    private String mPassword;
    private Server mServer;
    private Thread mManageTasksThread;

    public TaskManager() {
    }

    @Override
    public IBinder onBind(Intent intent) {
        return null;
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
            mServer.tryLogin(mEmail, mPassword);
        }
        /**manageTasks();**/
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
