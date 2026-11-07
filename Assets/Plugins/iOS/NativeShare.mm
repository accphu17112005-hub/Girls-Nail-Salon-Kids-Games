#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

extern "C" {
    void _iOS_ShareImage(const char* imagePath, const char* message) {
        NSString* pathStr = [NSString stringWithUTF8String:imagePath];
        NSString* msgStr = [NSString stringWithUTF8String:message];
        UIImage* image = [UIImage imageWithContentsOfFile:pathStr];

        NSMutableArray* items = [NSMutableArray array];
        if (msgStr) [items addObject:msgStr];
        if (image) [items addObject:image];

        UIActivityViewController* activityVC = [[UIActivityViewController alloc] initWithActivityItems:items applicationActivities:nil];
        UIViewController* rootVC = [UIApplication sharedApplication].keyWindow.rootViewController;

        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad) {
            activityVC.popoverPresentationController.sourceView = rootVC.view;
            activityVC.popoverPresentationController.sourceRect = CGRectMake(rootVC.view.bounds.size.width / 2, rootVC.view.bounds.size.height / 2, 0, 0);
        }

        [rootVC presentViewController:activityVC animated:YES completion:nil];
    }
}
