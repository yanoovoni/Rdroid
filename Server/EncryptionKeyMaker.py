#region -----------------Info-----------------
#Name:
#Version:
#By: Yaniv Sharon
#endregion -----------------Info-----------------

#region -----------------Imports-----------------
from Settings import *
from Printer import *
from Singleton import *
from Encryptor import *
from socket import *
from Crypto.PublicKey import RSA
from Crypto.Cipher import AES
#endregion -----------------Imports-----------------

#region -----------------Constants-----------------

#endregion -----------------Constants-----------------

#region -----------------Class-----------------


class EncryptionKeyMaker(object):
    __metaclass__ = Singleton
    printer = Printer()
    settings = Settings()
    __key = None

    def __init__(self):
        self.printer.printMessage(self.__class__.__name__, 'Generating RSA key.')
        self.__key = RSA.generate(int(self.settings.getSetting('RSA_bits')))
        self.printer.printMessage(self.__class__.__name__, 'RSA key generated.')

    def createEncryptor(self, phone_socket):
        return Encryptor(AES.new(b'01234567890123456789012345678901'))
        phone_socket.send()


#endregion -----------------Class-----------------