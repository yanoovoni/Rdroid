package com.yanoonigmail.rdroid.app;

import android.content.res.Resources;

import com.yanoonigmail.rdroid.R;

import static com.yanoonigmail.rdroid.R.string.protocol_client_header;
import static com.yanoonigmail.rdroid.R.string.protocol_client_login_announcement;
import static com.yanoonigmail.rdroid.R.string.protocol_client_login_password;
import static com.yanoonigmail.rdroid.R.string.protocol_client_login_email;
import static com.yanoonigmail.rdroid.R.string.protocol_client_task_request_announcement;
import static com.yanoonigmail.rdroid.R.string.protocol_parameter_separator;
import static com.yanoonigmail.rdroid.R.string.protocol_server_header;
import static com.yanoonigmail.rdroid.R.string.protocol_server_login_announcement;
import static com.yanoonigmail.rdroid.R.string.protocol_server_login_bool;
import static com.yanoonigmail.rdroid.R.string.protocol_server_login_success;

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
        String[] line_array = login_response.split(line_separator);
        if (line_array.length != 3)
            return false;
        if (!line_array[1].equals(resources.getString(protocol_server_login_announcement)))
            return false;
        if (!line_array[2].startsWith(resources.getString(protocol_server_login_bool) +
                resources.getString(protocol_parameter_separator)))
            return false;
        String[] line3_array;
        line3_array = line_array[2].split(resources.getString(protocol_parameter_separator));
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
                    resources.getString(R.string.protocol_client_task_request_exclude);
        }
        StringBuilder sb = new StringBuilder(message);
        sb.deleteCharAt(message.length() - 1);
        message = sb.toString();
        message += line_separator;
        return message;
    }
}
