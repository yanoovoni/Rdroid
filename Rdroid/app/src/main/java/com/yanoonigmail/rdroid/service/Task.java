package com.yanoonigmail.rdroid.service;

import android.content.res.Resources;
import android.os.Environment;
import android.util.Log;

import com.yanoonigmail.rdroid.ApplicationContext;
import com.yanoonigmail.rdroid.R;

import java.io.File;
import java.io.IOException;
import java.net.Socket;
import java.net.UnknownHostException;
import java.util.logging.SocketHandler;

import static com.yanoonigmail.rdroid.R.string.protocol_client_task_results_output_separator;
import static com.yanoonigmail.rdroid.R.string.protocol_parameter_separator;
import static com.yanoonigmail.rdroid.R.string.server_address;

/**
 * Created by yanoo on 19-Mar-16.
 */
public class Task {
    private String id;
    private String type;
    private String parameters;
    private Thread taskThread;
    private static Resources resources = ApplicationContext.getContext().getResources();
    private Server server = Server.getInstance();
    private ServerFileTransferor fileTransferor = ServerFileTransferor.getInstance();

    public Task(String id, String type, String parameters) {
        this.id = id;
        this.type = type;
        this.parameters = parameters;
        initTask();
    }

    public String getId() {
        return id;
    }

    public String getType() {
        return type;
    }

    public Thread getTaskThread() {
        return taskThread;
    }

    private void initTask() {
        taskThread = new Thread(new Runnable() {
            public void run() {
                String output = "";
                switch (type) {
                    case "GET_FILES_IN_FOLDER":
                        output = getFilesInFolder(parameters.split(resources.getString(protocol_parameter_separator))[1]);
                        break;
                    case "SET_FILE_TRANSFER_MODE":
                        try {
                            setFileTransferMode(parameters.split(resources.getString(protocol_parameter_separator))[1]);
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                }
                server.send(Protocol.taskResultsMessage(id, output));
            }
        });
        taskThread.start();
    }

    private String getFilesInFolder(String folder) {
        String filesString = "";
        File sdCardRoot = Environment.getExternalStorageDirectory();
        File yourDir = new File(sdCardRoot, folder);
        for (File f : yourDir.listFiles()) {
            String name;
            if (f.isFile()) {
                name = f.getName();
                Log.i("file name", name);
            }
            else {
                name = f.getName() + ":folder";
            }
            filesString += name + resources.getString(protocol_client_task_results_output_separator);
        }
        return filesString;
    }

    private void setFileTransferMode(String work) throws IOException {
        if (work.equals("true")) {
            fileTransferor.runCommunicationThread();
        }
        else {
            if (work.equals("false")) {
                fileTransferor.close();
            }
        }
    }
}
