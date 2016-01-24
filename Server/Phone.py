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
from Protocol import *
from socket import *
#endregion -----------------Imports-----------------

#region -----------------Constants-----------------

#endregion -----------------Constants-----------------

#region -----------------Class-----------------


class Phone(object):
    server = Server()
    settings = Settings()
    filter = Filter()
    encryption_key_maker = EncryptionKeyMaker()
    protocol = Protocol()
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
            message = self.recv()
            if self.filter.filter(message):
                self.server.send(message)

    def rawSend(self, message):
        phone_socket = self.getSocket()
        phone_socket.send(message)

    def rawRecv(self):
        phone_socket = self.getSocket()
        return phone_socket.recv(int(self.settings.getSetting('buffer_size')))

    def send(self, message):
        encryptor = self.getEncryptor()
        self.rawSend(encryptor.encrypt(message))

    def recv(self):
        encryptor = self.getEncryptor()
        return encryptor.decrypt(self.rawRecv())

    def establishConnection(self):
        self.__setEncryptor()
        self.__login()
        self.setReady()

    def checkLogin(self, login_message):
        login_message_line_list = login_message.split('\r\n')[2:]
        login_info_dict = {}
        for line in login_message_line_list:
            line = line.split(': ')
            login_info_dict[line[0]] = line[1]


    def setReady(self):
        self.__ready = True

    def getUsername(self):
        return self.__username

    def __setUsername(self, username):
        self.__username = username

    def isReady(self):
        return self.__ready

    def getNumber(self):
        return self.__number

    def getSocket(self):
        return self.__socket

    def closeThread(self):
        self.__close = True

    def getEncryptor(self):
        return self.__encryptor

    def __setEncryptor(self):
        self.__encryptor = self.encryption_key_maker.createEncryptor(self.getSocket())

    def __login(self):
        logged_in = False
        while not logged_in:
            login_message = self.recv()
            if self.filter.isLoginMessage(login_message):
                succesful, username = self.checkLogin(login_message)
                if succesful:
                    self.__setUsername(username)
                    self.setReady()


#endregion -----------------Class-----------------