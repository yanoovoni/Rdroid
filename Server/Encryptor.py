#region -----------------Info-----------------
#Name:
#Version:
#By: Yaniv Sharon
#endregion -----------------Info-----------------

#region -----------------Imports-----------------
from Settings import *
from Printer import *
#endregion -----------------Imports-----------------

#region -----------------Constants-----------------

#endregion -----------------Constants-----------------

#region -----------------Class-----------------


class Encryptor(object):
    __encryption_key = None

    def __init__(self, key):
        self.__encryption_key = key

    def encrypt(self, message):
        padded_message = self.__addPadding(message)
        encrypted_message = self.__encryption_key.encrypt(padded_message)
        return encrypted_message

    def decrypt(self, message):
        return self.__encryption_key.decrypt(message)

    def __addPadding(self, message):
        padding = (len(message) % 16) * '\x08'
        return message + padding

    def __removePadding(self, message):
        pass

#endregion -----------------Class-----------------