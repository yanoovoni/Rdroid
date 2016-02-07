#region -----------------Info-----------------
#Name:
#Version:
#By: Yaniv Sharon
#endregion -----------------Info-----------------

#region -----------------Imports-----------------
import os
from Singleton import *
from Printer import *
from threading import *
#endregion -----------------Imports-----------------

#region -----------------Constants-----------------

#endregion -----------------Constants-----------------

#region -----------------Class-----------------


class Settings(object):
    __metaclass__ = Singleton
    __settings_location = os.path.dirname(os.path.abspath(__file__)) + '\\Files\\server settings.cfg'
    __default_settings_location = os.path.dirname(os.path.abspath(__file__)) + '\\Files\\default server settings.cfg'
    __settings_dict = {}
    __printer = Printer()
    __read_lock = Semaphore(10)
    __write_lock = Lock()

    def __init__(self):
        if os.path.isfile(self.__settings_location):
            self.__loadSettings(self.__settings_location)
        elif os.path.isfile(self.__default_settings_location):
            self.__loadSettings(self.__default_settings_location)
            self.__updateSettingsFile()
        else:
            self.__printer.printMessage(self.__class__.__name__, 'No settings files found. The program will likely crush soon.')

    def getSetting(self, key):
        if self.__settings_dict.has_key(key):
            return self.__settings_dict[key]
        else:
            return None

    def setSetting(self, key, value):
        if self.__settings_dict.has_key(key):
            self.__settings_dict[key] = value
            self.__updateSettingsFile()
            return True
        else:
            return False

    def __loadSettings(self, location=__settings_location):
        self.__write_lock.acquire()
        self.__read_lock.acquire()
        self.__write_lock.release()
        settings_file = open(location, 'r')
        settings_list = settings_file.read()
        settings_file.close()
        self.__read_lock.release()
        settings_list = settings_list.split('\n')
        for setting in settings_list:
            if setting != '':
                if setting[0] != '#':
                    setting = setting.split('=', 1)
                    setting[1] = setting[1].replace('<CRLF>', '\r\n')
                    self.__settings_dict[setting[0]] = setting[1]
        return

    def __updateSettingsFile(self):
        new_settings_text = ''
        settings_template = open(self.__default_settings_location, 'r')
        settings_list = settings_template.read().split('\n')
        settings_template.close()
        for setting in settings_list:
            if setting != '':
                if setting[0] == '#':
                    new_settings_text += setting
                else:
                    setting = setting.split('=', 1)
                    new_settings_text += '%s=%s' % (setting[0], self.__settings_dict[setting[0]])
            new_settings_text += '\n'
        new_settings_text = new_settings_text[:-1]
        self.__write_lock.acquire()
        for i in xrange(10):
            self.__read_lock.acquire()
        settings_file = open(self.__settings_location, 'w')
        settings_file.write(new_settings_text)
        settings_file.close()
        for i in xrange(10):
            self.__read_lock.release()
        self.__write_lock.release()
        return


#endregion -----------------Class-----------------