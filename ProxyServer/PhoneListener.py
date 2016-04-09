#region -----------------Info-----------------
#Name: Phone Listener
#Version: 1.0
#By: Yaniv Sharon
#endregion -----------------Info-----------------

#region -----------------Imports-----------------
from Singleton import *
from Settings import *
from Printer import *
from PhoneManager import *
from socket import *
#endregion -----------------Imports-----------------

#region -----------------Class-----------------


class PhoneListener(object):
    # An object that simplifies the use of the listening socket of this proxy server.
    __metaclass__ = Singleton
    settings = Settings()
    printer = Printer()
    phone_manager = PhoneManager()
    __close = False # Specifies whether the thread that this object runs should close.

    def __init__(self):
        self.my_ip = self.settings.getSetting('my_ip')
        self.my_port = int(self.settings.getSetting('my_port'))
        self.listen_socket = socket()
        self.listen_socket.bind((self.my_ip, self.my_port))

    def runThread(self):
        # The main method that is supposed to run on a thread.
        self.listen_socket.listen(10)
        while not self.__close:
            client_socket, client_address = self.listen_socket.accept()
            client_ip = client_address[0]
            self.phone_manager.addPhone(client_socket, client_ip)
        self.listen_socket.close()

    def closeThread(self):
        # Tells the thread of this object to close.
        self.__close = True
        accept_trigger_socket = socket()
        accept_trigger_socket.connect(('127.0.0.1', self.my_port))
        accept_trigger_socket.close()

#endregion -----------------Class-----------------