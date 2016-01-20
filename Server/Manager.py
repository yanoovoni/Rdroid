#region -----------------Info-----------------
#Name:
#Version:
#By: Yaniv Sharon
#endregion -----------------Info-----------------

#region -----------------Imports-----------------
from Singleton import *
from Printer import *
from Settings import *
from PhoneListener import *
from Server import *
from PhoneManager import *
from threading import Thread
#endregion -----------------Imports-----------------

#region -----------------Constants-----------------

#endregion -----------------Constants-----------------

#region -----------------Class-----------------


class Manager(object):
    __metaclass__ = Singleton
    settings = Settings()
    printer = Printer()
    phone_listener = PhoneListener()
    server = Server()
    phone_manager = PhoneManager()
    __close = False

    def __init__(self):
        return

    def run(self):
        phone_listener_thread = Thread(target=self.phone_listener.runThread)
        phone_manager_thread = Thread(target=self.phone_manager.runThread)
        phone_listener_thread.start()
        phone_manager_thread.start()
        while not self.__close:
            user_input = raw_input()
            self.__inputResponse(user_input)

    def __inputResponse(self, user_input):
        if user_input == 'stop':
            self.closeThreads()

    def closeThreads(self):
        self.__closeThread()
        self.phone_listener.closeThread()
        self.phone_manager.closeThread()
        self.server.closeThreads()
        return

    def __closeThread(self):
        self.__close = True

#endregion -----------------Class-----------------