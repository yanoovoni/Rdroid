#region -----------------Info-----------------
#Name:
#Version:
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
    __phone_dict = {}
    __close = False

    def addPhone(self, phone_socket, phone_ip_address):
        phone = Phone(phone_socket, phone_ip_address, self.id_generator.generateId(), self)
        add_phone_thread = Thread(name='add_phone_thread', target=self.__addPhoneToDict, args=[phone])
        add_phone_thread.setDaemon(True)
        add_phone_thread.start()

    def deletePhone(self, phone_id):
        phone_dict = self.__phone_dict
        if phone_dict.has_key(phone_id):
            del phone_dict[phone_id]

    def getPhone(self, phone_id):
        self.printer.printMessage(self.__class__.__name__, 'phone_dict: %s' % (self.__phone_dict))
        if self.__phone_dict.has_key(phone_id):
            phone = self.__phone_dict[phone_id]
            return phone
        return None

    def runThread(self):
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
        self.__close = True

    def __addPhoneToDict(self, phone):
        phone.establishConnection()
        phone_ip_address = phone.getIp()
        phone_id = phone.getId()
        self.printer.printMessage(self.__class__.__name__, 'ip: %s, id: %s' % (phone_ip_address, phone_id))
        self.__phone_dict[phone_id] = phone
        phone_thread = Thread(name=('phone_thread-%s' % phone_id), target=phone.runThread)
        phone_thread.setDaemon(True)
        phone_thread.start()

    def __notifyDeletedPhone(self, phone_id):
        end_line = self.settings.getSetting('end_line')
        notification_message = self.settings.getSetting('disconnect_notification_message')
        notification_message += 'session_id:%s%s' % (phone_id, end_line)
        self.server.send(notification_message)


#endregion -----------------Class-----------------