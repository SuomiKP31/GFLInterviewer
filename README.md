# GFLInterviewer
Based on ImGUI.Net, a tool for convenient fanart making for Girls' Frontline related games.

The resources in the project is copyrighted to Sunborn Inc., under their intellectual property GFL.

请下载Release版本。

## 使用方法：
此程序使用Imgui作为用户界面，目的是根据输入的文案输出一列对话长图。
Resource/Avatar下存放png格式头像，建议尺寸150x150。不用专门调整尺寸，渲染时会放缩。

打开程序后，使用Project Creator创建项目，项目会被存放在Resources/Project
之后打开项目即可开始编辑。

- 使用 菜单-对话节点-添加对话节点创建第一个节点。
- 使用 菜单-Project-增加说话人创建说话人名。
- 使用 菜单-对话节点-添加旁白节点创建旁白。
- 点击 输出png 以将长图输出到Resources/Output

## 功能指引
- 在现存最后一个节点处点击“下一个节点” 会自动创建一个方向相对的对话节点。
- 对话节点-左/右会分别与一个说话人和头像相关联。如果节点由“下一个节点”自动创建，会自动填充说话人和头像栏，让一对一对话的编辑比较顺畅。
- 输出png-30分段 按钮 会把长图切分成30个一段的图片输出。过长的图片在一些平台会被二压导致无法阅读。当你的文案太长（超过四五十个节点）的时候可以使用。

## 现存Bug
因为Imgui对IME（Input Method Editor）的支持不好，使用中文输入法在InputText框会遇到一些问题。如果你使用退格键清除输入的拼音，Imgui的InputField也会接收到退格信号，删除掉你之前打好的字。
由于这个问题发生在底层的InputQueue，也不是Imgui当前需要紧急修复的问题，短期内应该没法解决。目前要使用的话建议先在外部打好草稿文案，再复制进这个程序生成长图。

## 输出样例

![Outputs...](https://user-images.githubusercontent.com/50021290/170855995-f7d34a1b-e123-49ab-977a-8042135a5eed.png)
