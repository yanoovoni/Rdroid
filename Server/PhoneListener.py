#region -----------------Info-----------------
#Name:
#Version:
#By: Yaniv Sharon
#endregion -----------------Info-----------------

#region -----------------Imports-----------------
from Singleton import *
from Settings import *
from Printer import *
from PhoneManager import *
from socket import *
#endregion -----------------Imports-----------------

#region -----------------Constants-----------------

#endregion -----------------Constants-----------------

#region -----------------Class-----------------


class PhoneListener(object):
    __metaclass__ = Singleton
    settings = Settings()
    printer = Printer()
    phone_manager = PhoneManager()
    close = False

    def __init__(self):
        self.my_ip = self.settings.getSetting('my_ip')
        self.my_port = int(self.settings.getSetting('my_port'))
        self.listen_socket = socket()
        self.listen_socket.bind((self.my_ip, self.my_port))

    def runThread(self):
        self.listen_socket.listen(10)
        while not self.close:
            client_socket, client_address = self.listen_socket.accept()
            self.phone_manager.addPhone(client_socket)
        self.listen_socket.close()

    def closeThread(self):
        self.close = True
        accept_trigger_socket = socket()
        accept_trigger_socket.connect(('127.0.0.1', self.my_port))
        accept_trigger_socket.close()

#endregion -----------------Class-----------------