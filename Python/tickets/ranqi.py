#!/usr/bin/env python3
#-*-coding:utf-8-*-
"""ranqi query via command-line.

Usage:
    ranqi <bianma> <pwd>

Example:
    ranqi 0188695 123456@



Visual Studio Code python settings
http://jingyan.baidu.com/article/00a07f38503a2b82d028dc26.html    
"""

from docopt import docopt
import requests
from prettytable import PrettyTable
import smtplib
from email.mime.text import MIMEText
from email.mime.multipart import MIMEMultipart
import datetime


class ranqiCollection(object):
    header = 'bm humin 年月 抄表日期 LastRead ThisRead 气量 price 应收 recvedAmt 违约金 rcvedPenalty isPayOver'.split()

    def __init__(self, bm, ranqiData):
        self.ranqiData = ranqiData
        self.bm = bm

    def pretty_print(self):
        pt = PrettyTable()
        pt._set_field_names(self.header)
        month_datas = self.ranqiData["C_dateList"][0]["DBSET"][0]["R"]
        
        for ranqiRecord in month_datas:
            itemRecord = ranqiRecord["C_feeRecord"][0]["DBSET"][0]["R"][0]
            resultItem = itemRecord["C_result"][0]["DBSET"][0]["R"][0]["C_locList"][0]["DBSET"][0]["R"][0]
            # print(resultItem)
            isPayOver = itemRecord['C_settleFlag'] # 是否结清
            # print(isPayOver)
            if isPayOver == "01":
                isPayOver = "".join(["\033[91m", "欠费", "\033[0m"])
            elif isPayOver=="02":
                isPayOver = "".join(["\033[91m", "部分欠费", "\033[0m"])
            elif isPayOver=="03":
                isPayOver = "".join(["\033[92m", "全部结清", "\033[0m"])
            elif isPayOver=="04":
                isPayOver = "坏账核销"
            else:
                isPayOver = "-"
            prettyRow = [self.bm, self.ranqiData["C_consName"], ranqiRecord["C_ym"], resultItem["C_endTime"][:10],
resultItem["C_lastRead"], resultItem["C_thisRead"], resultItem["C_tgq"], float(itemRecord["C_revblAmt"])/float(resultItem["C_tgq"]),
itemRecord["C_revblAmt"], itemRecord["C_rcvedAmt"], itemRecord["C_rcvalPenalty"], itemRecord["C_rcvedPenalty"], isPayOver]
            pt.add_row(prettyRow)
        print(pt)

        return pt


def sendMail(prettyTable, pwd):
    fromaddr = "xubo2004@126.com"
    toaddr = "398755692@qq.com"
    msg = MIMEMultipart()
    msg['From'] = fromaddr
    msg['To'] = toaddr
    msg['Subject'] = "燃气费 - {}".format(datetime.datetime.now())

    body = "{} \r\n{}".format(prettyTable, "http://www.cdgas.com/chaxun.jsp?yhbm=01886955")
    #msg.attach(MIMEText(body, 'html'))
    msg.attach(MIMEText(body, 'plain'))
    # message =
    try:
        smtpObj = smtplib.SMTP('smtp.126.com')
        smtpObj.set_debuglevel(1)
        smtpObj.ehlo()
        smtpObj.starttls()
        smtpObj.ehlo()
        smtpObj.login("xubo2004@126.com", pwd)
        smtpObj.sendmail(fromaddr, toaddr, msg.as_string())
        print("Successfully sent email")
    except:
        print("Error: unable to send email")

def search():
    arguments = docopt(__doc__)
    bm = arguments['<bianma>']
    pwd = arguments['<pwd>']
    #
    url = "http://www.cdgas.com/casesAction.do?method=chaxun"
    data = {"yhbm": bm}
    result = requests.post(url, data)
    jsonResult = result.json()
    gasFee = jsonResult["gasFee"]["MESSAGE"]["RESPONSE"][0]
    dbset = gasFee["DATA"][0]["DBSET"][0]
    
    ranqis = ranqiCollection(bm, dbset)
    pt = ranqis.pretty_print()
    sendMail(pt, pwd)

    # print(data)
    #print(jsonResult)


if __name__ == '__main__':
    search()
