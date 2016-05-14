#region -----------------Info-----------------
#Name: Phone
#Version: 1.0
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
import base64
#endregion -----------------Imports-----------------

#region -----------------Class-----------------


class Phone(object):
    server = Server()
    settings = Settings()
    printer = Printer()
    filter = Filter()
    encryption_key_maker = EncryptionKeyMaker()
    __ready = False # Tells if the phone is ready for communication.
    __socket = None # The socket of the session with the phone.
    __ip_address = '' # The ip address of the phone.
    __number = '' # The phone number of the connected phone (might be empty / useless).
    __encryptor = None # The encryptor that is used for communication with the phone.
    __close = False # Specifies whether the thread that this object runs should close.
    buffer_size = int(Settings().getSetting('buffer_size'))

    def __init__(self, phone_socket, phone_ip_address, phone_id, phone_manager):
        self.__socket = phone_socket
        self.__ip_address = phone_ip_address
        self.__phone_id = phone_id
        self.phone_manager = phone_manager

    def runThread(self):
        # The method that the thread runs.
        self.__notifyCreation()
        while not self.__close:
            message = self.recv()
            if self.filter.filter(message):
                message = '%s:%s' % (self.getID(), message)
                self.server.send(message)

    def raw_send(self, message):
        # Sends a message to the phone.
        message = base64.b64encode(message)
        message = str(len(message)) + ':' + message + '\n'
        print 'sent: ' + message
        self.getSocket().sendall(message)

    def raw_recv(self):
        # Receives a message from the phone.
        try:
            stay = True
            message_len = 0
            while stay:
                one_char = self.getSocket().recv(1)
                if one_char == '':
                    print 'got empty'
                    self.closeObject()
                    return None
                elif one_char == ':':
                    stay = False
                else:
                    try:
                        message_len = message_len * 10 + int(one_char)
                    except ValueError:
                        message_len = 0
            message = ''
            while len(message) < message_len:
                message += self.getSocket().recv(min(self.buffer_size, message_len - len(message)))
        except socket.error as e:
            print str(e)
            self.closeObject()
            return None
        except (IndexError, ValueError) as e:
            print str(e)
            return None
        if message[:-1] == '\n':
            return base64.b64decode(message[:-1])
        return base64.b64decode(message)

    def send(self, message):
        # encrypts and then sends a message to the phone.
        message = message.replace(self.settings.getSetting('new_line'), '\n')
        self.printer.printMessage(self.__class__.__name__, 'after edit to phone: ' + message)
        encryptor = self.getEncryptor()
        encrypted_message = encryptor.encrypt(message)
        self.printer.printMessage(self.__class__.__name__, 'after encryption to phone: ' + encrypted_message)
        self.raw_send(encrypted_message)

    def recv(self):
        # Receives a message from the phone and decrypts it.
        encryptor = self.getEncryptor()
        message = self.raw_recv()
        message = encryptor.decrypt(message)
        return message

    def establishConnection(self):
        # Handles important early communication with the phone (sets encryption).
        self.__setEncryptor()
        self.setReady()

    def setReady(self):
        # Makes it so that the phone would count as ready.
        self.__ready = True

    def isReady(self):
        # Returns whether the phone is ready or not.
        return self.__ready

    def getNumber(self):
        # Returns the phone number.
        return self.__number

    def getSocket(self):
        # Returns the socket of the current session.
        return self.__socket

    def getIp(self):
        # Returns the ip of the phone.
        return self.__ip_address

    def getID(self):
        # Returns the ID that was given to the phone.
        return self.__phone_id

    def getEncryptor(self):
        # Returns the encryptor object that this object uses.
        return self.__encryptor

    def closeObject(self):
        # Closes the thread that this object runs and removes the object from the phone manager (removes the object from the memory completely).
        self.__closeThread()
        self.phone_manager.deletePhone(self.getID())

    def __notifyCreation(self):
        # Send the server a message to notify it that this phone connected and it's ID.
        new_line = self.settings.getSetting('new_line')
        my_id = self.getID()
        notification_message = self.settings.getSetting('id_notification_message')
        notification_message += 'session_id:%s%s' % (my_id, new_line)
        self.server.send(notification_message)

    def __closeThread(self):
        # Closes the thread that runs on this object.
        self.__close = True

    def __setEncryptor(self):
        # Sets an encryptor object for this object.
        self.__encryptor = self.encryption_key_maker.createEncryptor(self)


#endregion -----------------Class-----------------