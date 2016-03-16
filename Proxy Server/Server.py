#region -----------------Info-----------------
#Name: Server
#Version: 1.0
#By: Yaniv Sharon
#endregion -----------------Info-----------------

#region -----------------Imports-----------------
from Settings import *
from Printer import *
from Singleton import *
from Queue import *
from threading import *
import time
import socket
import base64
#endregion -----------------Imports-----------------

#region -----------------Class-----------------


class Server(object):
    # An object used as an interface for communicating with the server.
    __metaclass__ = Singleton
    settings = Settings()
    printer = Printer()
    __server_socket = socket.socket() # The socket that is connected to the server.
    __input_queue = Queue() # A queue that holds the messages that were sent from the server.
    __output_queue = Queue() # A queue that holds the messages that are going to be sent to the server.
    __connect_lock = Lock() # A lock used to make sure that this object will not try to reconnect to the server twice at the same time.
    __close = False # Specifies whether the thread that this object runs should close.

    def __init__(self):
        self.__connectToServer()
        self.runThreads()

    def runThreads(self):
        # A method that runs the thread that are supposed to run on this object.
        server_listener_thread = Thread(name='server_listener_thread', target=self.__serverReceiverThread)
        server_listener_thread.setDaemon(True)
        server_listener_thread.start()
        output_sender_thread = Thread(name='output_sender_thread', target=self.__outputSenderThread)
        output_sender_thread.setDaemon(True)
        output_sender_thread.start()

    def closeThreads(self):
        # Closes the thread that run on this object.
        self.__close = True
        self.send('closing')
        self.__server_socket.close()
        self.__addInput('closing')

    def send(self, message):
        # Adds a message to the output queue so it will be sent when possible.
        message = message.replace(self.settings.getSetting('new_line'), '\n')
        self.__output_queue.put(message)

    def recv(self):
        # Returns the first message from the input queue. blocks until it has a value to return (if the queue is empty).
        while self.__input_queue.empty():
            time.sleep(0.1)
        return self.__input_queue.get()

    def __serverReceiverThread(self):
        # A method that is supposed to run on a thread that receives messages from the server and adds them to the input queue.
        buffer_size = int(self.settings.getSetting('buffer_size'))
        while not self.__close:
            try:
                message = self.__server_socket.recv(buffer_size)
                message = base64.b64decode(message)
                self.__addInput(message)
            except socket.error:
                self.__connectToServer()

    def __outputSenderThread(self):
        # A method that is supposed to run on a thread that sends messages from the output queue to the server.
        while not self.__close:
            try:
                self.__send(self.__output_queue.get())
            except socket.error:
                self.__connectToServer()

    def __addInput(self, input):
        # Adds a message to the input queue.
        self.__input_queue.put(input)

    def __send(self, message):
        # sends a message to the server.
        message = base64.b64encode(message)
        self.__server_socket.send(message)

    def __connectToServer(self):
        # connects to the server.
        acquired = self.__connect_lock.acquire()
        if acquired:
            try:
                self.__send('blah')
            except socket.error:
                self.printer.printMessage(self.__class__.__name__, 'Connecting to server')
                self.__server_socket = socket.socket()
                ip = self.settings.getSetting('main_server_ip')
                port = int(self.settings.getSetting('main_server_port'))
                connected = False
                while not connected:
                    connected = True
                    try:
                        self.__server_socket.connect((ip, port))
                    except socket.error:
                        connected = False
                self.printer.printMessage(self.__class__.__name__, 'Connected to server')
            self.__connect_lock.release()


#endregion -----------------Class-----------------