#region -----------------Info-----------------
#Name:
#Version:
#By: Yaniv Sharon
#endregion -----------------Info-----------------

#region -----------------Imports-----------------
from Settings import *
from Printer import *
from EncryptionKeyMaker import *
from Encryptor import *
from Filter import *
from Server import *
from socket import *
#endregion -----------------Imports-----------------

#region -----------------Constants-----------------

#endregion -----------------Constants-----------------

#region -----------------Class-----------------


class Phone(object):
    server = Server()
    filter = Filter()
    __ready = False
    __socket = None
    __username = ''
    __number = ''
    __encryptor = None
    __close = False


    def __init__(self, phone_socket):
        self.__socket = phone_socket

    def runThread(self):
        while not self.__close:
            message = self.__socket.recv()
            if self.filter.filter(message):
                self.server.send(message)

    def send(self, message):
        self.__socket.send(message)

    def establishConnection(self):
        pass

    def setReady(self):
        self.__ready = True

    def getUsername(self):
        return self.__username

    def isReady(self):
        return self.__ready

    def getNumber(self):
        return self.__number

    def getSocket(self):
        return self.__socket

    def closeThread(self):
        self.__close = True

    def __setEncryption(self):
        pass


#endregion -----------------Class-----------------