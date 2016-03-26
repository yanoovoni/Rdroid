package com.yanoonigmail.rdroid.service;

import android.content.res.Resources;

import com.yanoonigmail.rdroid.R;
import com.yanoonigmail.rdroid.ApplicationContext;

import java.util.ArrayList;
import java.util.Arrays;

import static com.yanoonigmail.rdroid.R.string.protocol_client_header;
import static com.yanoonigmail.rdroid.R.string.protocol_client_login_announcement;
import static com.yanoonigmail.rdroid.R.string.protocol_client_login_password;
import static com.yanoonigmail.rdroid.R.string.protocol_client_login_email;
import static com.yanoonigmail.rdroid.R.string.protocol_client_task_output_announcement;
import static com.yanoonigmail.rdroid.R.string.protocol_client_task_output_id;
import static com.yanoonigmail.rdroid.R.string.protocol_client_task_output_output;
import static com.yanoonigmail.rdroid.R.string.protocol_client_task_request_announcement;
import static com.yanoonigmail.rdroid.R.string.protocol_server_task_response_task_parameters_separator;
import static com.yanoonigmail.rdroid.R.string.protocol_server_task_response_announcement;
import static com.yanoonigmail.rdroid.R.string.protocol_parameter_separator;
import static com.yanoonigmail.rdroid.R.string.protocol_server_header;
import static com.yanoonigmail.rdroid.R.string.protocol_server_login_announcement;
import static com.yanoonigmail.rdroid.R.string.protocol_server_login_bool;
import static com.yanoonigmail.rdroid.R.string.protocol_server_login_success;
import static com.yanoonigmail.rdroid.R.string.protocol_server_task_response_task;

/**
 * Created by Yaniv on 27-Feb-16.
 */
public class Protocol {
    private static String line_separator = System.getProperty("line.separator");
    private static Resources resources = ApplicationContext.getContext().getResources();

    public static boolean isServerMessage(String message) {
        return message.startsWith(resources.getString(protocol_server_header));
    }

    public static String loginRequest(String email, String password) {
        String message;
        message = resources.getString(protocol_client_header) +
                line_separator;
        message += resources.getString(protocol_client_login_announcement) +
                line_separator;
        message += resources.getString(protocol_client_login_email) +
                resources.getString(protocol_parameter_separator) +
                email +
                line_separator;
        message += resources.getString(protocol_client_login_password) +
                resources.getString(protocol_parameter_separator) +
                password +
                line_separator;
        return message;
    }

    public static boolean loginResponseBool(String login_response) {
        if (!isServerMessage(login_response))
            return false;
        String[] lineArray = login_response.split(line_separator);
        if (lineArray.length != 3)
            return false;
        if (!lineArray[1].equals(resources.getString(protocol_server_login_announcement)))
            return false;
        if (!lineArray[2].startsWith(resources.getString(protocol_server_login_bool) +
                resources.getString(protocol_parameter_separator)))
            return false;
        String[] line3_array;
        line3_array = lineArray[2].split(resources.getString(protocol_parameter_separator));
        return line3_array[1].equals(resources.getString(protocol_server_login_success));
    }

    public static String taskRequest(String[] excluded_ids) {
        String message;
        message = resources.getString(protocol_client_header) +
                line_separator;
        message += resources.getString(protocol_client_task_request_announcement) +
                line_separator;
        message += resources.getString(R.string.protocol_client_task_request_exclude) +
                line_separator;
        for (String excluded_id : excluded_ids) {
            message += excluded_id +
                    resources.getString(R.string.protocol_client_task_request_exclude_separator);
        }
        StringBuilder sb = new StringBuilder(message);
        sb.deleteCharAt(message.length() - 1);
        message = sb.toString();
        message += line_separator;
        return message;
    }

    /**
     * Turns the response packet from thew server into an array of Task objects.
     * @param message The response packet that was received.
     * @return A list of given Task objects.
     */
    public static Task[] taskResponse(String message) {
        ArrayList<Task> taskList = new ArrayList<>();
        if (isServerMessage(message)) {
            String[] lineArray = message.split(line_separator);
            if (lineArray[1].equals(resources.getString(protocol_server_task_response_announcement))) {
                String[] parameterLineArray = Arrays.copyOfRange(lineArray, 2, lineArray.length);
                for (String line : parameterLineArray) {
                    if (line.startsWith(resources.getString(protocol_server_task_response_task))) {
                        String[] taskParamsArray = line.split(resources.getString(protocol_parameter_separator), 1)[1].split(resources.getString(protocol_server_task_response_task_parameters_separator));
                        if (taskParamsArray.length == 3) {
                            try {
                                taskList.add(taskCreator(taskParamsArray));
                            } catch (BadInputException e) {
                                e.printStackTrace();
                            }
                        }
                    }
                }
            }
        }
        Task[] taskArray = new Task[taskList.size()];
        taskList.toArray(taskArray);
        return taskArray;
    }

    /**
     * Made for taskResponse.
     * @param taskParamsArray The array of strings that describe the task object.
     * @return A task object.
     */
    protected static Task taskCreator(String[] taskParamsArray) throws BadInputException {
        String id = "";
        String type = "";
        String parameters = "";
        for (String param : taskParamsArray) {
            String[] paramDetailsCouple = param.split(resources.getString(protocol_parameter_separator));
            switch (paramDetailsCouple[0]) {
                case "id":
                    id = paramDetailsCouple[1];
                    break;
                case "type":
                    type = paramDetailsCouple[1];
                    break;
                case "parameters":
                    parameters = paramDetailsCouple[1];
            }
        }
        if (id.equals("") || type.equals("") || parameters.equals("")) {
            throw new BadInputException("Missing task parameters.");
        }
        return new Task(id, type, parameters);
    }

    public static String taskOutputMessage(String id, String output) {
        String outputString = resources.getString(protocol_client_header) +
                line_separator;
        outputString += resources.getString(protocol_client_task_output_announcement) +
                line_separator;
        outputString += resources.getString(protocol_client_task_output_id) +
                resources.getString(protocol_parameter_separator) +
                id +
                line_separator;
        outputString += resources.getString(protocol_client_task_output_output) +
                resources.getString(protocol_parameter_separator) +
                output +
                line_separator;
        return outputString;
    }

    static class BadInputException extends Exception {
        public BadInputException(String message) {
            super(message);
        }
    }
}

