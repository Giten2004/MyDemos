import logging

# load my module
import my_module
import loggingSetting

# load the logging configuration
# logging.config.fileConfig('logging.ini')
loggingSetting.setup_logging('logging.json', logging.DEBUG, 'LOG_CFG')

logger = logging.getLogger(__name__)
logger.debug("Init logging finished.")

my_module.foo()
bar = my_module.Bar()
bar.bar()