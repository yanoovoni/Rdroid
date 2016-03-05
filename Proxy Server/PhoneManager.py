#region -----------------Info-----------------
#Name: Phone Manager
#Version: 1.0
#By: Yaniv Sharon
#endregion -----------------Info-----------------

#region -----------------Imports-----------------
from Singleton import *
from Settings import *
from Printer import *
from Server import *
from Phone import *
from IDGenerator import *
from threading import Thread
#endregion -----------------Imports-----------------

#region -----------------Class-----------------


class PhoneManager(object):
    __metaclass__ = Singleton
    settings = Settings()
    printer = Printer()
    server = Server()
    id_generator = IDGenerator()
    __phone_dict = {} # A dictionary that is used to remember the connected phones sessions by session IDs.
    __close = False # Specifies whether the thread that this object runs should close.

    def addPhone(self, phone_socket, phone_ip_address):
        # Starts a thread that adds a phone to the phone dictionary.
        phone = Phone(phone_socket, phone_ip_address, self.id_generator.generateId(), self)
        add_phone_thread = Thread(name='add_phone_thread', target=self.__addPhoneToDictThread, args=[phone])
        add_phone_thread.setDaemon(True)
        add_phone_thread.start()

    def deletePhone(self, phone_id):
        # Removes a phone from the dictionary.
        phone_dict = self.__phone_dict
        if phone_dict.has_key(phone_id):
            self.__notifyDeletedPhone(phone_id)
            del phone_dict[phone_id]

    def getPhone(self, phone_id):
        # Returns the phone that has the given ID.
        phone_dict = self.__phone_dict
        self.printer.printMessage(self.__class__.__name__, 'phone_dict: %s' % phone_dict)
        if phone_dict.has_key(phone_id):
            phone = phone_dict[phone_id]
            return phone
        return None

    def runThread(self):
        # The main method of this object that is supposed to run on a new thread.
        while not self.__close:
            server_message = self.server.recv()
            if ':' in server_message:
                phone_id, message = server_message.split(':', 1)
                phone = self.getPhone(phone_id)
                if phone is not None:
                    phone.send(message)
                else:
                    self.__notifyDeletedPhone(phone_id)

    def closeThread(self):
        # Tells the thread of this object to close.
        self.__close = True

    def __addPhoneToDictThread(self, phone):
        # A method that is supposed to run on a new thread that adds a phone to the dictionary and start it's thread.
        phone.establishConnection()
        phone_ip_address = phone.getIp()
        phone_id = phone.getID()
        self.printer.printMessage(self.__class__.__name__, 'ip: %s, id: %s' % (phone_ip_address, phone_id))
        self.__phone_dict[phone_id] = phone
        phone_thread = Thread(name=('phone_thread-%s' % phone_id), target=phone.runThread)
        phone_thread.setDaemon(True)
        phone_thread.start()

    def __notifyDeletedPhone(self, phone_id):
        # Notifies the server of phones that were deleted from the dictionary (most likely disconnected) and their IDs.
        new_line = self.settings.getSetting('new_line')
        notification_message = self.settings.getSetting('disconnect_notification_message')
        notification_message += 'session_id:%s%s' % (phone_id, new_line)
        self.server.send(notification_message)


#endregion -----------------Class-----------------