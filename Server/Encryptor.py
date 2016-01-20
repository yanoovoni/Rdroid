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
        return self.__encryption_key.encrypt(message)

    def decrypt(self, message):
        return self.__encryption_key.decrypt(message)


#endregion -----------------Class-----------------