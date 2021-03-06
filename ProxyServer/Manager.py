#region -----------------Info-----------------
#Name: Manager
#Version: 1.0
#By: Yaniv Sharon
#endregion -----------------Info-----------------

#region -----------------Imports-----------------
from Singleton import *
from Printer import *
from Settings import *
from Server import *
from PhoneListener import *
from PhoneManager import *
from threading import Thread
#endregion -----------------Imports-----------------

#region -----------------Class-----------------


class Manager(object):
    __metaclass__ = Singleton
    settings = Settings()
    printer = Printer()
    server = Server()
    phone_listener = PhoneListener()
    phone_manager = PhoneManager()
    __close = False # Specifies whether the thread that this object runs should close.

    def run(self):
        # Runs the main code of the object.
        phone_listener_thread = Thread(name='phone_listener_thread', target=self.phone_listener.runThread)
        phone_listener_thread.setDaemon(True)
        phone_listener_thread.start()
        phone_manager_thread = Thread(name='phone_manager_thread', target=self.phone_manager.runThread)
        phone_manager_thread.setDaemon(True)
        phone_manager_thread.start()
        while not self.__close:
            user_input = raw_input()
            self.__inputResponse(user_input)

    def closeThreads(self):
        # Tells the open threads to close.
        self.__closeThread()
        self.phone_listener.closeThread()
        self.phone_manager.closeThread()
        self.server.closeThreads()
        return

    def __inputResponse(self, user_input):
        # Analizes the given input and calls the matching method.
        if user_input == 'stop':
            self.closeThreads()

    def __closeThread(self):
        # Tells the thread of this object to close.
        self.__close = True

#endregion -----------------Class-----------------