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
from threading import Thread
#endregion -----------------Imports-----------------

#region -----------------Constants-----------------

#endregion -----------------Constants-----------------

#region -----------------Class-----------------


class PhoneManager(object):
    __metaclass__ = Singleton
    settings = Settings()
    printer = Printer()
    server = Server()
    __phone_dict = {}
    __close = False

    def addPhone(self, phone_socket, phone_ip_address):
        phone = Phone(phone_socket, phone_ip_address)
        add_phone_thread = Thread(name='add_phone_thread', target=self.__addPhoneToDict, args=[phone])
        add_phone_thread.setDaemon(True)
        add_phone_thread.start()

    def getPhone(self, ip_address):
        return self.__phone_dict[ip_address]

    def runThread(self):
        while not self.__close:
            server_message = self.server.recv()
            if server_message is not None:
                if ':' in server_message:
                    phone_ip_address, message = server_message.split(':', 1)
                    phone = self.getPhone(phone_ip_address)
                    phone.send(message)

    def __addPhoneToDict(self, phone):
        phone.establishConnection()
        phone_ip_address = phone.getIp()
        self.printer.printMessage(self.__class__.__name__, 'ip: ' + phone_ip_address)
        self.__phone_dict[phone_ip_address] = phone
        phone_thread = Thread(name=('phone_thread-%s' % phone_ip_address), target=phone.runThread)
        phone_thread.setDaemon(True)
        phone_thread.start()

    def closeThread(self):
        self.__close = True


#endregion -----------------Class-----------------