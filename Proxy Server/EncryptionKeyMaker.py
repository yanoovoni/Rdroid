#region -----------------Info-----------------
#Name: Encryption Key Maker
#Version: 1.0
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

#region -----------------Class-----------------


class EncryptionKeyMaker(object):
    # This class is used for creating symmetric encryption keys for communicating with phones.
    __metaclass__ = Singleton
    printer = Printer()
    settings = Settings()
    __encryption_key = None # The asymmetric encryption key that is used for the process of creating the symmetric key.

    def __init__(self):
        self.printer.printMessage(self.__class__.__name__, 'Generating RSA key.')
        self.__key = RSA.generate(int(self.settings.getSetting('RSA_bits')))
        self.printer.printMessage(self.__class__.__name__, 'RSA key generated.')

    def createEncryptor(self, phone_socket):
        # Communicates with the given socket and creates a symmetric key that is used by both sides.
        return Encryptor(AES.new(b'01234567890123456789012345678901'))
        #phone_socket.send()


#endregion -----------------Class-----------------