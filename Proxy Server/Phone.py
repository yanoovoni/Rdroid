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
import socket
#endregion -----------------Imports-----------------

#region -----------------Class-----------------


class Phone(object):
    server = Server()
    settings = Settings()
    filter = Filter()
    encryption_key_maker = EncryptionKeyMaker()
    protocol = Protocol()
    __ready = False
    __socket = None
    __ip_address = ''
    __number = ''
    __encryptor = None
    __close = False


    def __init__(self, phone_socket, phone_ip_address, phone_id, phone_manager):
        self.__socket = phone_socket
        self.__ip_address = phone_ip_address
        self.__phone_id = phone_id
        self.phone_manager = phone_manager

    def runThread(self):
        self.__notifyCreation()
        while not self.__close:
            try:
                message = self.recv()
                if self.filter.filter(message):
                    message = '%s:%s' % (self.getId(), message)
                    self.server.send(message)
            except socket.error:
                self.closeObject()

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
        self.setReady()

    def setReady(self):
        self.__ready = True

    def isReady(self):
        return self.__ready

    def getNumber(self):
        return self.__number

    def getSocket(self):
        return self.__socket

    def getIp(self):
        return self.__ip_address

    def getId(self):
        return self.__phone_id

    def closeObject(self):
        self.__closeThread()
        self.phone_manager.deletePhone(self.getId())

    def getEncryptor(self):
        return self.__encryptor

    def __notifyCreation(self):
        end_line = self.settings.getSetting('end_line')
        my_id = self.getId()
        notification_message = self.settings.getSetting('id_notification_message')
        notification_message += 'session_id:%s%s' % (my_id, end_line)
        self.server.send(notification_message)

    def __closeThread(self):
        self.__close = True

    def __setEncryptor(self):
        self.__encryptor = self.encryption_key_maker.createEncryptor(self.getSocket())


#endregion -----------------Class-----------------