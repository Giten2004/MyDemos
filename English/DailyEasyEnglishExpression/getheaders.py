#!/usr/bin/python
# -*- coding: UTF-8 -*- 

import os

basepath = "F:\BaiduYunDownload\Daily Easy English Expression"
current_files = os.listdir(basepath)


with open('ecubedHeader.md', 'a') as filehandler:
    for file_name in current_files:
        full_file_name = os.path.join(basepath, file_name)
         
        if os.path.isdir(full_file_name):
            continue
        strContent = "## " + file_name.replace(" -- 3 Minute English Lesson", "").replace(".mp4", "")
        filehandler.writelines(strContent)
        filehandler.write("\n")
    filehandler.close()
