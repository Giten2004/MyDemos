import matplotlib.pyplot as plt


plt.plot([1,2,3], [1,2,3], 'go-', label='line 1', linewidth=2)
plt.plot([1,2,3], [1,4,9], 'rs',  label='line 2')

plt.axis([0, 4, 0, 10])


plt.legend()

# plt.plot([1,2,3,4], [1,4,9,16], 'ro')
# plt.axis([0, 6, 0, 20])
plt.show()