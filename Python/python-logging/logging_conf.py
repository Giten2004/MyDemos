import logging
import logging.config
import mylib

logging.config.fileConfig('logging.conf')

# create logger
logger = logging.getLogger(__name__)

# 'application' code
logger.debug('debug message')
logger.info('info message')
logger.warn('warn message')
mylib.do_something()
logger.error('error message')
logger.critical('critical message')
