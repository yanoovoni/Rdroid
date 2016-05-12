#region -----------------Info-----------------
#Name: Encryptor
#Version: 1.0
#By: Yaniv Sharon
#endregion -----------------Info-----------------

#region -----------------Imports-----------------
from Settings import *
from Printer import *
#endregion -----------------Imports-----------------

#region -----------------Class-----------------


class Encryptor(object):
    # An object that simplifies the use of symmetric keys.
    printer = Printer()
    settings = Settings()
    __encryption_key = None # The symmetric key that the object uses.

    def __init__(self, encryption_key):
        self.__encryption_key = encryption_key

    def encrypt(self, message):
        # Adds padding to the message and encrypts it.
        padded_message = addPadding(message)
        encrypted_message = self.__encryption_key.encrypt(padded_message)
        return encrypted_message

    def decrypt(self, message):
        # Decrypts the message and removes the padding.
        try:
            print 'after base64: ' + message
            padded_message = self.__encryption_key.decrypt(message)
            print 'after decryption: ' + padded_message
            unpadded_message = removePadding(padded_message)
            print 'after unpadding: ' + unpadded_message
        except (ValueError, TypeError, IndexError) as e:
            print str(e)
            unpadded_message = message
        return unpadded_message

def addPadding(message):
        # Adds padding to the message
        pad_len = 16 - len(message) % 16
        padded_message = message.ljust(len(message) + pad_len, chr(pad_len))
        return padded_message

def removePadding(message):
    # Removes padding from the message.
    pad_len = ord(message[-1])
    if pad_len >= 16:
        return message
    real_message = message[:-pad_len]
    return real_message

#endregion -----------------Class-----------------