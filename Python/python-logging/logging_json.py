import logging
import logging.config
import json
import mylib

"""
configuration demo
https://gist.github.com/pmav99/49c01313db33f3453b22
"""
with open("logging.json", "r") as fd:
        logging.config.dictConfig(json.load(fd))

# create logger
logger = logging.getLogger(__name__)

# 'application' code
logger.debug('debug message')
logger.info('info message')
logger.warn('warn message')
mylib.do_something()
logger.error('error message')
logger.critical('critical message')
