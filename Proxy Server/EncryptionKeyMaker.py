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
from Crypto.Cipher import PKCS1_v1_5
from Crypto.Cipher import AES
#endregion -----------------Imports-----------------

#region -----------------Class-----------------


class EncryptionKeyMaker(object):
    # This class is used for creating symmetric encryption keys for communicating with phones.
    __metaclass__ = Singleton
    printer = Printer()
    settings = Settings()
    __pure_encryption_key = None # The asymmetric encryption key that is used for the process of creating the symmetric key.
    __encryption_key = None

    def __init__(self):
        self.printer.printMessage(self.__class__.__name__, 'Generating RSA key.')
        self.__pure_encryption_key = RSA.generate(int(self.settings.getSetting('RSA_bits')))
        self.__encryption_key = PKCS1_v1_5.new(self.__pure_encryption_key)
        self.printer.printMessage(self.__class__.__name__, 'RSA key generated.')

    def createEncryptor(self, phone):
        # Communicates with the given socket and creates a symmetric key that is used by both sides.
        errors_param = None
        public_key = self.__pure_encryption_key.publickey().exportKey(format='DER')
        print 'public key: ' + public_key
        phone.raw_send(public_key)
        encrypted_key = phone.raw_recv()
        print 'key: ' + encrypted_key
        print 'key len: ' + str(len(encrypted_key))
        phone_key = self.__encryption_key.decrypt(encrypted_key, errors_param)
        print 'phone key: ' + phone_key
        return Encryptor(AES.new(phone_key))


#endregion -----------------Class-----------------