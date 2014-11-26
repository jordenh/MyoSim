
#include <gtest\gtest.h>
#include "PoseSim.h"

// This test is named "testPass", and belongs to the "testCase"
// test case.
TEST(testCase,testPass) {
    ASSERT_EQ(1, 1);
}

// This test is named "testFail", and belongs to the "testCase"
// test case.
TEST(testCase, testFail) {
    ASSERT_EQ(1, 2);
}

// example test.
TEST(ex1, name1) {
    myoSim::Pose *pose = new myoSim::Pose();

    ASSERT_EQ(myoSim::Pose::Type::unknown, pose->type());
    ASSERT_EQ(1, 1);

    delete pose;
}