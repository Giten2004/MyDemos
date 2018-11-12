#!/usr/bin/env python

import logmatic
import logging
import socket

"""
https://github.com/logmatic/logmatic-python
"""
logger = logging.getLogger()

handler = logging.StreamHandler()
handler.setFormatter(logmatic.JsonFormatter(extra={"hostname":socket.gethostname()}))

logger.addHandler(handler)
logger.setLevel(logging.INFO)

test_logger = logging.getLogger("test")
test_logger.info("classic message", extra={"special": "value", "run": 12})
logger.warn("No user currently authenticated.", extra={"customer": "my_beloved_customer", "login_name": "foo@bar.com"})
