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
    __encryption_key = None # The symmetric key that the object uses.

    def __init__(self, encryption_key):
        self.__encryption_key = encryption_key

    def encrypt(self, message):
        # Adds padding to the message and encrypts it.
        padded_message = self.__addPadding(message)
        encrypted_message = self.__encryption_key.encrypt(padded_message)
        return encrypted_message

    def decrypt(self, message):
        # Decrypts the message and removes the padding.
        padded_message = self.__encryption_key.decrypt(message)
        return self.__removePadding(padded_message)

    def __addPadding(self, message):
        # Adds padding to the message
        padding = (len(message) % 16) * '\x08'
        return message + padding

    def __removePadding(self, message):
        # Removes padding from the message.
        return message

#endregion -----------------Class-----------------