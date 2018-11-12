#!/usr/bin/env python
#-*-coding:utf-8-*-

## Python玩机器学习简易教程
## http://shujuren.org/article/484.html
##开始时间：2017年8月24日
##结束时间：2017年9月16日
## 第一步：设置环境
import sys
print("Python version: %s" % sys.version)
import numpy
print("numpy version: %s" % numpy.__version__)

import matplotlib
print("matplotlib version: %s" % matplotlib.__version__)

import pandas
print("pandas version: %s" % pandas.__version__)

import sklearn
print("sklearn version: %s" % sklearn.__version__)

## 第二步：导入所需库
import numpy as np 
import pandas as pd 
from sklearn.model_selection import train_test_split
from sklearn import preprocessing
from sklearn.ensemble import RandomForestRegressor
from sklearn.pipeline import make_pipeline
from sklearn.model_selection import GridSearchCV
from sklearn.metrics import mean_squared_error, r2_score
from sklearn.externals import joblib

## 第三步：加载数据集
dataset_url = "http://mlr.cs.umass.edu/ml/machine-learning-databases/wine-quality/winequality-red.csv"
data = pd.read_csv(dataset_url, sep = ";")
print(data.head())
print(data.shape)
print(data.describe())

## 第四步：数据集划分
y = data.quality
X = data.drop("quality", axis = 1)
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size = 0.2, random_state = 123, stratify=y)

## 第五步：数据预处理
## 对训练集的所有特征进行标准化处理
pipeline = make_pipeline(preprocessing.StandardScaler(), RandomForestRegressor(n_estimators=100))

## 第六步：参数调优
print(pipeline.get_params())
hyperparameters = { 'randomforestregressor__max_features' : ['auto', 'sqrt', 'log2'],
                  'randomforestregressor__max_depth': [None, 5, 3, 1]}

## 第七步：模型优化(交叉验证)
clf = GridSearchCV(pipeline, hyperparameters, cv=10)
clf.fit(X_train, y_train)
print(clf.best_params_)

## 第八步：全数据拟合
print(clf.refit)

## 第九步：模型评估
y_pred = clf.predict(X_test)
print(r2_score(y_test, y_pred))
print(mean_squared_error(y_test, y_pred))

## 第十步：模型保存
joblib.dump(clf, 'rf_regressor.pkl')
clf2 = joblib.load('rf_regressor.pkl')

# 加载模型预测新的数据集
clf2.predict(X_test)